using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace The203.CollectionJson.Core.Model
{
    public interface ICJException : ISerializable
    {
	    string Code { get; }
	    string Title { get; }
	    string UserMessage { get; }
    }
}
