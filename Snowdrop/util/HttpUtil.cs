// Snowdrop - Download, Install and Update
// Copyright (c) 2017 Christoph Heich
// See the LICENSE file in the project root for more information.

using System;
using System.Net;
using System.Net.Sockets;

namespace Snowdrop.util
{
    class HttpUtil
    {
        public static bool PingHost(string url, int port)
        {
            try
            {
                TcpClient client = new TcpClient(url, port);
                client.Close();
                return true;
            }
            catch (Exception ex)
            {
                LoggingUtil.Exception(String.Format("Connection Exception: {0}", ex.Message));
                return false;
            }
        }

        public static bool WebUrlExist(string url)
        {
            bool result = false;

            WebRequest webRequest = WebRequest.Create(url);
            webRequest.Timeout = 1200; // miliseconds
            webRequest.Method = "HEAD";

            HttpWebResponse response = null;

            try
            {
                response = (HttpWebResponse)webRequest.GetResponse();
                result = true;
            }
            catch (WebException webException)
            {
                LoggingUtil.Exception(url + " doesn't exist: " + webException.Message);
            }
            finally
            {
                if (response != null)
                {
                    response.Close();
                }
            }

            return result;
        }
    }
}
