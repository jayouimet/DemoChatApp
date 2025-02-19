using System.Net;
using ChatApp.Common.Errors.Abstracts;
using ChatApp.Common.Errors.Interfaces;

namespace ChatApp.Common.Errors;

public class BadRequestError(string key, string message) : BaseHttpError(key, message, (int)HttpStatusCode.BadRequest);