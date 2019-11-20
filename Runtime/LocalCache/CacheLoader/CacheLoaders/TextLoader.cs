
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
namespace LocalLoadProcess
{
    public class TextLoader : FileLoader
    {
        public string Text;
        public TextLoader()
        {
        }

        public TextLoader(LoaderConfig config) : base(config)
        {
        }

        public override void Execute()
        {
            if (Async)

            {
                base.Execute();
            }
            else
            {
                try
                {
                    Text = File.ReadAllText(Config.LoadPath);
                }
                catch (Exception e)
                {
                    ErrorStr = e.ToString();
                    Error();
                }
                Complete();
            }
        }
        public override void Complete()
        {
            if (Async)
            {
                if (Data != null)
                {
                    Text = Encoding.UTF8.GetString(Data);
                }
            }
            base.Complete();
        }
    }
}