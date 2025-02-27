using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace SMK.Data.Dto
{
    public class LogicRtnModel<T>
    {
        private string errMsg;
        /// <summary>
        /// ture for no errors
        /// </summary>
        public bool IsSuccess { get; set; } = true;

        public string Msg { get; set; }

        public string ErrMsg
        {
            get => this.errMsg;

            set
            {
                this.errMsg = value;
                this.IsSuccess = false;
            }
        }

        public T Data { get; set; }

        public object ExtendData { get; set; }

        [JsonIgnore]
        public string StackTrace { get; set; }


        public LogicRtnModel(string errMsg = "", string msg = "")
        {
            this.ErrMsg = errMsg;
            this.Msg = msg;
            this.IsSuccess = string.IsNullOrEmpty(errMsg);
        }

        public LogicRtnModel(MsgType type, string extraMsg = "")
        {
            var msg = SharedMessage.Instance.GetMsg(type);
            if (!string.IsNullOrEmpty(extraMsg))
            {
                msg = $"{msg}({extraMsg})";
            }
            if (type.ToString().Contains("Fail"))
            {
                this.ErrMsg = msg;
            }
            else
            {
                this.Msg = msg;
            }
            this.IsSuccess = string.IsNullOrEmpty(this.ErrMsg);
        }


        public LogicRtnModel<T> SetMsgType(MsgType type)
        {
            var msg = SharedMessage.Instance.GetMsg(type);
            if (type.ToString().Contains("Fail"))
            {
                this.ErrMsg = msg;
            }
            else
            {
                this.Msg = msg;
            }
            this.IsSuccess = string.IsNullOrEmpty(this.ErrMsg);
            return this;
        }
        /// <summary>
        /// 更新屬性至目標model
        /// </summary>
        /// <typeparam name="T1"></typeparam>
        /// <param name="model"></param>
        public void extends<T1>(LogicRtnModel<T1> model)
        {
            this.IsSuccess = model.IsSuccess;
            
            this.ExtendData = model.ExtendData;
            if (!string.IsNullOrEmpty(model.ErrMsg))
            {
                this.ErrMsg = model.errMsg;
            }
            this.Msg = model.Msg;
        }
    }



    public enum MsgType
    {
        QueryFail,

        CreateFail,
        CreateSuccess,

        SaveFail,
        SaveSuccess,

        RemoveFail,
        RemoceSuccess,

        ProcessFail

    }


    public class SharedMessage
    {

        private static SharedMessage instance = new SharedMessage();
        private Dictionary<MsgType, string> messages = new Dictionary<MsgType, string>();

        public static SharedMessage Instance { get { return instance; } private set { } }


        public string GetMsg(MsgType type)
        {
            return messages[type];
        }


        private SharedMessage()
        {
            messages.Add(MsgType.QueryFail, "查詢失敗");
            messages.Add(MsgType.CreateFail, "新增失敗");
            messages.Add(MsgType.SaveFail, "儲存失敗");
            messages.Add(MsgType.RemoveFail, "刪除失敗");

            messages.Add(MsgType.CreateSuccess, "新增成功");
            messages.Add(MsgType.SaveSuccess, "更新成功");
            messages.Add(MsgType.RemoceSuccess, "成功刪除");
        }


    }
}
