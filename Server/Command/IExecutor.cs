using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Command
{
    public interface IExecutor
    {
        /// <summary>
        /// 命令文本
        /// </summary>
        string[] CommandText { get; }
        /// <summary>
        /// 和命令文本吻合则执行
        /// </summary>
        /// <param name="parameters">包含服务端,客户端Socket,及传输的字节流</param>
        /// <param name="cmd"></param>
        void Excute(AsyncParameters parameters,string cmd);
    }
}
