using UnityEngine;
using System;

namespace hg.ApiWebKit
{
	public enum HttpFaultWhenCondition
	{
		Unset,
		Is,
		IsNot
	}

	public class HttpCallbacks<OT>
	{
		public Action<OT,hg.ApiWebKit.core.http.HttpResponse> done; 
		public Action<OT,hg.ApiWebKit.core.http.HttpResponse> fail;
		public Action<OT,hg.ApiWebKit.core.http.HttpResponse> always;
	}

	public enum ApiBehaviorStatus
	{
		NONE,
		SUCCESS,
		FAILURE
	}

	[Flags]
	public enum MappingDirection
	{
		NONE = 1,
		REQUEST = 2,
		RESPONSE = 4,
		ALL = 8
	}

	public enum LogSeverity
	{
		ERROR,
		WARNING,
		INFO,
		VERBOSE
	}
	
	public enum HttpRequestState 
	{       
	    NONE,
	    IDLE,
	    BUSY,
	    STARTED,
	    COMPLETED,
	    TIMEOUT,
	    ERROR,
	    CANCELLED,
	    DISCONNECTED
	}

	public enum HttpStatusCode
    {
		Unknown = -1,
        Continue = 100,
        SwitchingProtocols = 101,
        Processing = 102,
        Checkpoint = 103,
        OK = 200,
        Created = 201,
        Accepted = 202,
        NonAuthoritativeInformation = 203,
        NoContent = 204,
        ResetContent = 205,
        PartialContent = 206,
        MultipleStatus = 207,
        IMUsed = 226,
        MultipleChoices = 300,
        MovedPermanently = 301,
        Found = 302,
        SeeOther = 303,
        NotModified = 304,
        UseProxy = 305,
        SwitchProxy = 306,
        TemporaryRedirect = 307,
        ResumeIncomplete = 308,
        BadRequest = 400,
        Unauthorized = 401,
        PaymentRequired = 402,
        Forbidden = 403,
        NotFound = 404,
        MethodNotAllowed = 405,
        NotAcceptable = 406,
        ProxyAuthenticationRequired = 407,
        RequestTimeout = 408,
        Conflict = 409,
        Gone = 410,
        LengthRequired = 411,
        PreconditionFailed = 412,
        RequestEntityTooLarge = 413,
        RequestUriTooLong = 414,
        UnsupportedMediaType = 415,
        RequestedRangeNotSatisfiable = 416,
        ExpectationFailed = 417,
        ImATeapot = 418,
        UnprocessableEntity = 422,
        Locked = 423,
        FailedDependency = 424,
        UnorderedCollection = 425,
        UpgradeRequired = 426,
        NoResponse = 444,
        RetryWith = 449,
        BlockedByWindowsParentalControls = 450,
        ClientClosedRequest = 499,
        InternalServerError = 500,
        NotImplemented = 501,
        BadGateway = 502,
        ServiceUnavailable = 503,
        GatewayTimeout = 504,
        HttpVersionNotSupported = 505,
        VariantAlsoNegotiates = 506,
        InsufficientStorage = 507,
        BandwidthLimitExceeded = 509,
        NotExtended = 510
    }
}