using System;
using System.Text;
using Message;
using UnityEngine;

namespace DefaultNamespace.EventSystem.Events
{
    public class NetworkEvent : Event
    {
                //编码
        public static byte[] Encode(NetworkEvent msaBase)
        {
            string s = JsonUtility.ToJson(msaBase);
            return Encoding.UTF8.GetBytes(s);
        }
        //解码
        public static NetworkEvent Decode(string protoName,byte[] bytes,int offset,int count)
        {
            string s = Encoding.UTF8.GetString(bytes,offset,count);
            string space = "Message.";
            string className = space + protoName;
            NetworkEvent msgBase = (NetworkEvent)JsonUtility.FromJson(s,Type.GetType(className));
            return msgBase;
        }
        
        //编译协议名
        /// <summary>
        /// 包括了协议名和协议名长度
        /// </summary>
        /// <param name="msgBase"></param>
        /// <returns></returns>
        public static byte[] EncodeName(NetworkEvent msgBase)
        {
            //名字和长度
            byte[] nameByte = Encoding.UTF8.GetBytes(msgBase.EventName);
            Int16 len = (Int16)nameByte.Length;
            //申请byte数组
            byte[] bytes = new byte[2+len];
            
            bytes[0] = (byte)(len%256);
            bytes[1] = (byte)(len/256);
            //组装名字
            Array.Copy(nameByte,0,bytes,2,len);
            
            return bytes;
        }
        
        //解码协议名
        /// <summary>
        /// 
        /// </summary>
        /// <param name="protoName"></param>
        /// <param name="bytes"></param>
        /// <param name="offset"></param> 用于数组的偏移，有时候传入的数据并不是长度信息并未存在数组的0处位置，可能会发生一些偏移，我们需要的是这个长度开始的地方
        /// <param name="count"></param>
        /// <returns></returns>
        public static string DecodeName( byte[] bytes, int offset, out int count)
        {
            count = 0;

            if (offset +2 > bytes.Length)
            {
                return "";
            }
            //读取长度
            Int16 len = (Int16)((bytes[offset+1] << 8)| bytes[offset]);
            //长度必须够
            if (offset + 2 + len > bytes.Length)
            {
                return "";
            }
            //解析
            count = 2 + len;
            string name = Encoding.UTF8.GetString(bytes,offset+2,len);
            return name;
        }
    }
}