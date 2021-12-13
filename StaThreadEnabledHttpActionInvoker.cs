using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Threading.Tasks.Schedulers;
using System.Web;
using System.Web.Http.Controllers;

namespace XLWinServiceAPI
{

    public class StaThreadEnabledHttpActionInvoker : ApiControllerActionInvoker
    {
        public override Task<HttpResponseMessage> InvokeActionAsync(HttpActionContext context, CancellationToken cancellationToken)
        {
            return Task.Factory.StartNewSta(() => base.InvokeActionAsync(context, cancellationToken).Result);
        }
    }
}
