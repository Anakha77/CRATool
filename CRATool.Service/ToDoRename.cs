using System;
using System.IO;
using System.Net.Sockets;
using System.Text;
using log4net;

namespace CRATool.Service
{
    public class ToDoRename
    {
        public void Listening(TcpListener server, ILog log)
        {
            try
            {
                var serverSideClient = server.AcceptTcpClient();
                var serverSideClientStream = serverSideClient.GetStream();
                var serverSideClientStreamReader = new StreamReader(serverSideClientStream);
                var serverSideClientStreamWriter = new StreamWriter(serverSideClientStream);

                string inputLine;
                var request = new StringBuilder();

                while (!string.IsNullOrEmpty(inputLine = serverSideClientStreamReader.ReadLine()))
                {
                    request.AppendLine(inputLine);
                }
                log.Info($"RECIEVED REQUEST : {request}");

                var response = GetResponsecontent(request);

                serverSideClientStreamWriter.Write(response);
                serverSideClientStreamWriter.Flush();

                log.Info($"RESPONSE SENT : {response}");

                serverSideClient.Close();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message, ex);
            }
        }

        private static string GetResponsecontent(StringBuilder request)
        {
            var response = new StringBuilder("HTTP/1.1 200 OK");
            response.AppendLine();
            response.AppendLine();
            response.AppendLine("<!DOCTYPE HTML PUBLIC \" -//IETF//DTD HTML 2.0//EN\">");
            response.AppendLine("<http>");
            response.AppendLine("<head>");
            response.AppendLine("</head>");
            response.AppendLine("<body>");
            response.AppendLine($"<h2>Uri={GetUri(request.ToString())}</h2>");
            response.AppendLine("</body>");
            response.AppendLine("</http>");
            return response.ToString();
        }

        private static string GetUri(string request)
        {
            var byteArray = Encoding.UTF8.GetBytes(request);
            var memStream = new MemoryStream(byteArray);
            var streamReader = new StreamReader(memStream);
            var requestLine = streamReader.ReadLine();
            if (requestLine == null) return "";
            var requestLineComponents = requestLine.Split(' ');
            return requestLineComponents[1];
        }
    }
}
