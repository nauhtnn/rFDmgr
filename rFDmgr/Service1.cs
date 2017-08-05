using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace rFDmgr
{
    public partial class Service1 : ServiceBase
    {
        Server2 mServer;

        public Service1()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            mServer = new Server2(SrvrBufHndl);
            mServer.SrvrPort = 23822;
            Task.Run(() => mServer.Start());
        }

        protected override void OnStop()
        {
        }

        public bool SrvrBufHndl(byte[] buf, out byte[] outMsg)
        {
            outMsg = null;
            return false;
        }
    }
}
