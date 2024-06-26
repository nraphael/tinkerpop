﻿#region License

/*
 * Licensed to the Apache Software Foundation (ASF) under one
 * or more contributor license agreements.  See the NOTICE file
 * distributed with this work for additional information
 * regarding copyright ownership.  The ASF licenses this file
 * to you under the Apache License, Version 2.0 (the
 * "License"); you may not use this file except in compliance
 * with the License.  You may obtain a copy of the License at
 *
 *     http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing,
 * software distributed under the License is distributed on an
 * "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY
 * KIND, either express or implied.  See the License for the
 * specific language governing permissions and limitations
 * under the License.
 */

#endregion

using System;

namespace Gremlin.Net.Driver.Messages
{
    /// <summary>
    ///     Represents the various status codes that Gremlin Server returns.
    /// </summary>
    public enum ResponseStatusCode
    {
        /// <summary>
        ///     The server successfully processed a request to completion - there are no messages remaining in this
        ///     stream.
        /// </summary>
        Success = 200,
        
        /// <summary>
        ///     The server processed the request but there is no result to return (e.g. an Iterator with no elements)
        ///     - there are no messages remaining in this stream.
        /// </summary>
        NoContent = 204,
        
        /// <summary>
        ///     The server successfully returned some content, but there is more in the stream to arrive - wait for a
        ///     SUCCESS to signify the end of the stream.
        /// </summary>
        PartialContent = 206,

        /// <summary>
        ///     The request attempted to access resources that the requesting user did not have access to.
        /// </summary>
        Unauthorized = 401,

        /// <summary>
        ///     The server could authenticate the request, but will not fulfill it.  This is a general purpose code that
        ///     would typically be returned if the request is authenticated but not authorized to do what it is doing.
        /// </summary>
        Forbidden = 403,

        /// <summary>
        ///     A challenge from the server for the client to authenticate its request.
        /// </summary>
        Authenticate = 407,

        /// <summary>
        ///     Indicates that too many requests have been sent in a given amount of time.
        /// </summary>
        TooManyRequests = 429,

        /// <summary>
        ///    The request message contains objects that were not serializable on the client side.
        /// </summary>
        ClientSerializationError = 497,

        /// <summary>
        ///     The request message was not properly formatted which means it could not be parsed at all or the "op" code
        ///     was not recognized such that Gremlin Server could properly route it for processing. Check the message format
        ///     and retry the request.
        /// </summary>
        MalformedRequest = 498,

        /// <summary>
        ///     The request message was parseable, but the arguments supplied in the message were in conflict or incomplete.
        ///     Check the message format and retry the request.
        /// </summary>
        InvalidRequestArguments = 499,

        /// <summary>
        ///     A general server error occurred that prevented the request from being processed.
        /// </summary>
        ServerError = 500,

        /// <summary>
        ///     A server error that is produced when the fail()-step is triggered.
        /// </summary>
        ServerErrorFailStep = 595,

        /// <summary>
        ///     A server error that indicates that the client should retry the request. A graph will typically return this error
        ///     when a transaction fails due to a locking exception or some other sort of concurrent modification. In other
        ///     words, the request was likely valid but the state of the server at the particular time the request arrived
        ///     could not be processed to success, but could be at a later moment.
        /// </summary>
        ServerErrorTemporary = 596,

        /// <summary>
        ///     The request submitted for processing evaluated by the server with errors and could not be processed.
        ///     Check the script or remote traversal submitted for errors or other problems and then resubmit.
        /// </summary>
        ServerEvaluationError = 597,

        /// <summary>
        ///     The server exceeded one of the timeout settings for the request and could therefore only partially responded
        ///     or did not respond at all.
        /// </summary>
        ServerTimeout = 598,

        /// <summary>
        ///     The server was not capable of serializing an object that was returned from the script supplied on the request.
        ///     Either transform the object into something Gremlin Server can process within the script or install mapper
        ///     serialization classes to Gremlin Server.
        /// </summary>
        ServerSerializationError = 599
    }

    internal static class ResponseStatusCodeExtensions
    {
        public static bool IndicatesError(this ResponseStatusCode statusCode)
        {
            switch (statusCode)
            {
                case ResponseStatusCode.Success:
                case ResponseStatusCode.NoContent:
                case ResponseStatusCode.PartialContent:
                case ResponseStatusCode.Authenticate:
                    return false;
                case ResponseStatusCode.Unauthorized:
                case ResponseStatusCode.Forbidden:
                case ResponseStatusCode.TooManyRequests:
                case ResponseStatusCode.ClientSerializationError:
                case ResponseStatusCode.MalformedRequest:
                case ResponseStatusCode.InvalidRequestArguments:
                case ResponseStatusCode.ServerError:
                case ResponseStatusCode.ServerErrorFailStep:
                case ResponseStatusCode.ServerErrorTemporary:
                case ResponseStatusCode.ServerEvaluationError:
                case ResponseStatusCode.ServerTimeout:
                case ResponseStatusCode.ServerSerializationError:
                    return true;
                default:
                    throw new ArgumentOutOfRangeException(nameof(statusCode), statusCode, null);
            }
        }
    }
}