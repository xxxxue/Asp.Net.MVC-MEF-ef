using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.ComponentModel.Composition.Primitives;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http.Dependencies;

namespace MefBase
{
    public class MefDependencySolver : IDependencyResolver, System.Web.Mvc.IDependencyResolver
    {
        private readonly ComposablePartCatalog _catalog;
        private const string MefContainerKey = "MefCommonContainerKey";

        public MefDependencySolver(ComposablePartCatalog catalog)
        {
            _catalog = catalog;
        }

        public CompositionContainer Container
        {
            get
            {
                CompositionContainer container;
                
                if (!HttpContext.Current.Items.Contains(MefContainerKey))
                {
                    container = new CompositionContainer(_catalog, CompositionOptions.DisableSilentRejection);
                    HttpContext.Current.Items.Add(MefContainerKey, container);

                    //释放资源
                    HttpContext.Current.DisposeOnPipelineCompleted(container);
                }
                else
                {
                    container = (CompositionContainer)HttpContext.Current.Items[MefContainerKey];
                }
                return container;
            }
        }

        #region IDependencyResolver Members

        public object GetService(Type serviceType)
        {
            var contractName = AttributedModelServices.GetContractName(serviceType);
            return Container.GetExportedValueOrDefault<object>(contractName);
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            return Container.GetExportedValues<object>(serviceType.FullName);
        }

        public IDependencyScope BeginScope()
        {
            return this;
        }
        /// <summary>
        /// 释放内存资源
        /// </summary>
        public void Dispose()
        {
            //调用带参数的Dispose方法，释放托管和非托管资源
            Dispose(true);
            //手动调用了Dispose释放资源，那么析构函数就是不必要的了，这里阻止GC调用析构函数
            GC.SuppressFinalize(this);

        }

        /// <summary>
        /// protected的Dispose方法，保证不会被外部调用。
        /// 传入bool值disposing以确定是否释放托管资源
        /// </summary>
        /// <param name="disposing">是否释放资源</param>
        private void Dispose(bool disposing)
        {
            if (!disposing) return;
            Container.Dispose();
        }

        #endregion
    }
}
