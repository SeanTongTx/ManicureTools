using SeanLib.Core.Event;
using SeanLib.Core.Sequence;

namespace LocalLoadProcess
{
    public abstract class CacheLoaderBase<T> : CommonProcess
    {
        public bool Async;
        public CacheLoaderBase()
        {
        }
        public CacheLoaderBase(LoaderConfig config)
        {
            this.Config = config;
        }
        public T Data;
        public new Signal<CacheLoaderBase<T>> OnComplete = new Signal<CacheLoaderBase<T>>();
        public new Signal<CacheLoaderBase<T>> OnError = new Signal<CacheLoaderBase<T>>();
        public LoaderConfig Config;
        public override void Complete()
        {
            OnComplete.Dispatch(this);
            base.Complete();
        }
        public override void Error()
        {
            OnError.Dispatch(this);
            base.Error();
        }
    }
}