using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VSBaseAngular.Models {
    public enum ResultCode {
        Ok,
        Error,
        Warning,
    }

    public class Result<T> where T : new() {
        
        public Result() {
            this.Messages = new List<string>();
        }

        public Result(T value) : this() {
            this.Value = value;
        }   

        public T Value { get; set; }
        public ResultCode Code { get; set; }
        public List<string> Messages { get; set; }

        public Result<T> AddMessage(string message) {
            if (this.Messages == null) this.Messages = new List<string>();
            this.Messages.Add(message);
            return this;
        }
    }
}