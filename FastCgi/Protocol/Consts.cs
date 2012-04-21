﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ByteArray = FastCgi.ImmutableArray.ImmutableArray<byte>;

namespace FastCgi.Protocol
{
	/// <summary>
	/// Defines the possible roles a FastCGI application may play
	/// </summary>
	public enum Role : ushort
	{
        /// <summary>
        /// A Responder FastCGI application has the same purpose as a CGI/1.1 program:
        /// It receives all the information associated with an HTTP request and generates
        /// an HTTP response. 
        /// </summary>
		Responder = 1,
        /// <summary>
        /// An Authorizer FastCGI application receives all the information associated with
        /// an HTTP request and generates an authorized/unauthorized decision.
        /// In case of an authorized decision the Authorizer can also associate name-value pairs
        /// with the HTTP request; when giving an unauthorized decision the Authorizer sends a
        /// complete response to the HTTP client. 
        /// </summary>
		Authorizer = 2,
        /// <summary>
        /// A Filter FastCGI application receives all the information associated with an
        /// HTTP request, plus an extra stream of data from a file stored on the Web server,
        /// and generates a "filtered" version of the data stream as an HTTP response. 
        /// </summary>
		Filter = 3
	};

	/// <summary>
	/// Possible statuses a request may declare when completed
	/// </summary>
	public enum ProtocolStatus : byte
	{
		/// <summary>
		/// Normal end of request
		/// </summary>
		RequestComplete = 0,
		/// <summary>
		/// Rejecting a new request. This happens when a Web server sends concurrent
		/// requests over one connection to an application that is designed to process
		/// one request at a time per connection.
		/// </summary>
		CantMpxConn = 1,
		/// <summary>
		/// Rejecting a new request. This happens when the application runs out of
		/// some resource, e.g. database connections. 
		/// </summary>
		Overloaded = 2,
		/// <summary>
		/// Rejecting a new request. This happens when the Web server has specified
		/// a role that is unknown to the application.
		/// </summary>
		UnknownRole = 3
	};

	/// <summary>
	/// Error codes
	/// </summary>
	public enum ErrorCodes
	{
		UnsupportedVersion = -2,
		ProtocolError = -3,
		ParamsError = -4,
		CallSeqError = -5,
	}
	
	public enum RecordType
	{
		Application,
		Management
	}

	public enum MessageType : byte
	{
        /// <summary>
        /// No message type specified
        /// </summary>
		None = 0,
		/// <summary>
		/// Message sent from the web server to begin a request
		/// </summary>
		BeginRequest = 1,
		/// <summary>
		/// Message sent form the web server to abort a request
		/// </summary>
		AbortRequest = 2,
		/// <summary>
		/// Message sent form the fastcgi server to end a request
		/// </summary>
		EndRequest = 3,
		/// <summary>
		/// Message sent from the web server supply request parameters
		/// </summary>
		Params = 4,
		/// <summary>
		/// Message sent from the web server supply request input
		/// </summary>
		StandardInput = 5,
        /// <summary>
        /// Message sent form the fastcgi server containing the reply content
        /// </summary>
		StandardOutput = 6,
        /// <summary>
        /// Message sent form the fastcgi server containing the reply content in case of errors
        /// </summary>
		StandardError = 7,
		/// <summary>
		/// Message sent from the web server supply request extra data
		/// </summary>
		Data = 8,
        /// <summary>
        /// Message sent from the web server to request fastcgi application parameters
        /// </summary>
		GetValues = 9,
        /// <summary>
        /// Message sent from the fastcgi application in reply to a <see cref="MessageType.GetValues"/> request
        /// </summary>
		GetValuesResult = 10,
        /// <summary>
        /// Message of a unknown type
        /// </summary>
		UnknownType = 11
	}

	public static class Consts
	{
		/// <summary>
		/// The version of the FastCGI protocol that this implementation adheres to
		/// </summary>
		public const int Version = 1;

		/// <summary>
		/// All FastCGI records will be a multiple of this many bytes
		/// </summary>
		public const ushort ChunkSize = 8;

		/// <summary>
		/// Maximum size of the data inside a message body
		/// </summary>
		public const ushort MaxMessageBodySize = 0xFFFF;

        public const int SuggestedBufferSize = ChunkSize * 2 + MaxMessageBodySize;

		/*
		* Return codes for Process* functions
		*/
		public static int def_FCGIStreamRecord    = 0;
		public static int def_FCGISkip        = 1;
		public static int def_FCGIBeginRecord = 2;
		public static int def_FCGIMgmtRecord = 3;
	}

    public static class Utils
    {
        public static ushort ReadUint16(ByteArray data, int offset)
        {
            return (ushort)((data[offset] << 8) + data[offset + 1]);
        }

        public static ushort ReadUint16(byte[] data, int offset)
        {
            return (ushort)((data[offset] << 8) + data[offset + 1]);
        }
    }
}
