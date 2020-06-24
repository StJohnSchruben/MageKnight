

using System;
using System.Globalization;
using System.IO;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Dispatcher;
using System.Xml;
using log4net;

namespace ServerHost
{
    /// <summary>
    /// A server-side message inspector that logs all incoming and outgoing messages.
    /// </summary>
    public class LoggingMessageInspector : IDispatchMessageInspector
    {
        /// <summary>
        /// The log.
        /// </summary>
        private readonly ILog log = LogManager.GetLogger(typeof(LoggingMessageInspector));

        /// <summary>
        /// Called after an inbound message has been received but before the message is dispatched to the
        /// intended operation.
        /// </summary>
        /// <param name="request">The request message.</param>
        /// <param name="channel">The incoming channel.</param>
        /// <param name="instanceContext">The current service instance.</param>
        /// <returns>
        /// The object used to correlate state. This object is passed back in the <see cref="BeforeSendReply" /> method.
        /// </returns>
        public object AfterReceiveRequest(ref Message request, IClientChannel channel, InstanceContext instanceContext)
        {
            if (request == null)
            {
                return null;
            }

            var buffer = request.CreateBufferedCopy(int.MaxValue);
            request = buffer.CreateMessage();

            var message = buffer.CreateMessage();

            StringWriter stringWriter = null;
            try
            {
                stringWriter = new StringWriter(CultureInfo.InvariantCulture);

                var xmlWriter = XmlWriter.Create(
                    stringWriter,
                    new XmlWriterSettings
                    {
                        Indent = true,
                        IndentChars = "    "
                    });

                message.WriteMessage(xmlWriter);

                xmlWriter.Flush();

                this.log.DebugFormat("Service received inbound message:\r\n{0}", stringWriter);
            }
            catch (Exception e)
            {
                this.log.Warn("Logging inbound message threw an exception.", e);
            }
            finally
            {
                stringWriter?.Dispose();
            }

            return null;
        }

        /// <summary>
        /// Called after the operation has returned but before the reply message is sent.
        /// </summary>
        /// <param name="reply">The reply message. This value is <c>null</c> if the operation is one way.</param>
        /// <param name="correlationState">The correlation object returned from the <see cref="AfterReceiveRequest" /> method.</param>
        public void BeforeSendReply(ref Message reply, object correlationState)
        {
            if (reply == null)
            {
                return;
            }

            var buffer = reply.CreateBufferedCopy(int.MaxValue);
            reply = buffer.CreateMessage();

            var message = buffer.CreateMessage();
            StringWriter writer = null;

            try
            {
                writer = new StringWriter(CultureInfo.InvariantCulture);

                var xmlWriter = XmlWriter.Create(
                    writer,
                    new XmlWriterSettings
                    {
                        Indent = true,
                        IndentChars = "    "
                    });

                message.WriteMessage(xmlWriter);

                xmlWriter.Flush();

                this.log.DebugFormat("Service is sending outbound message:\r\n{0}", writer);
            }
            catch (Exception e)
            {
                this.log.Warn("Logging outbound message threw an exception.", e);
            }
            finally
            {
                writer?.Dispose();
            }
        }
    }
}