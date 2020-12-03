using Resume.Domain.Response;
using Resume.Grpc.Protos.Football;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Resume.Application.ProtoAdapters
{
    public static class ProtoMessageResult_Result_Adapter
    {
        public static ProtoMessageResult Map(Result result)
        {
            if (result == null)
            {
                result = new Result();
                result.SetError("Failed to map to proto type because the argument supplied to the adapter was null");
            }

            ProtoMessageResult vtr = new ProtoMessageResult()
            {
                Succeeded = result.Succeeded
            };

            if (result?.Messages?.Any() ?? false)
            {
                vtr.Messages.AddRange(result.Messages);
            }
            return vtr;
        }

        public static ProtoMessageResult Map(Result result)
        {
            if (result == null)
            {
                result = new Result();
                result.SetError("Failed to map to proto type because the argument supplied to the adapter was null");
            }

            ProtoMessageResult vtr = new ProtoMessageResult()
            {
                Succeeded = result.Succeeded
            };

            if (result?.Messages?.Any() ?? false)
            {
                vtr.Messages.AddRange(result.Messages);
            }
            return vtr;
        }
    }
}
