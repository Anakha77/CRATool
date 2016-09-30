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
            var serverSideClient = server.AcceptTcpClient();
            var serverSideClientStream = serverSideClient.GetStream();
            var serverSideClientStreamReader = new StreamReader(serverSideClientStream);
            var serverSideClientStreamWriter = new StreamWriter(serverSideClientStream);

            var response = string.Empty;

            try
            {
                var request = new StringBuilder();

                var inputLine = serverSideClientStreamReader.ReadLine();
                if (string.IsNullOrEmpty(inputLine)) return;

                if (!inputLine.EndsWith("HTTP/1.1"))
                    throw new InvalidOperationException(Properties.Resources.Erreur_Protocole);

                var method = inputLine.Split(' ')[0];
                var uri = inputLine.Split(' ')[1];

                if (string.IsNullOrEmpty(uri))
                    throw new InvalidOperationException(string.Format(Properties.Resources.Erreur_Uri, uri));

                if (string.IsNullOrEmpty(method))
                    throw new InvalidOperationException(string.Format(Properties.Resources.Erreur_Methode, method));

                request.AppendLine(inputLine);

                while (!string.IsNullOrEmpty(inputLine = serverSideClientStreamReader.ReadLine()))
                {
                    request.AppendLine(inputLine);
                }
                log.Info($"RECIEVED REQUEST : {request}");

                response = CreateResponseText(200, "OK", GetSimpleResponseBody($"Uri={uri}"));
            }
            catch (Exception ex)
            {
                log.Error(ex.Message, ex);
                response = CreateResponseText(500, "ERREUR_INTERNE", "Une erreur est survenue");
            }
            finally
            {
                serverSideClientStreamWriter.Write(response);
                serverSideClientStreamWriter.Flush();

                log.Info($"RESPONSE SENT : {response}");

                serverSideClient.Close();
            }
        }

        private static string CreateResponseText(int statusCode, string statusMessage, string responseBody)
        {
            var response = new StringBuilder($"HTTP/1.1 {statusCode} {statusMessage}");
            response.AppendLine();
            response.AppendLine();
            response.AppendLine(responseBody);
            return response.ToString();
        }

        private static string GetSimpleResponseBody(string simpleText)
        {
            var body = new StringBuilder();
            body.AppendLine("<!DOCTYPE HTML PUBLIC \" -//IETF//DTD HTML 2.0//EN\">");
            body.AppendLine("<http>");
            body.AppendLine("<head>");
            body.AppendLine("</head>");
            body.AppendLine("<body>");
            body.AppendLine($"<h2>{simpleText}</h2>");
            body.AppendLine("</body>");
            body.AppendLine("</http>");
            return body.ToString();
        }
    }
}
