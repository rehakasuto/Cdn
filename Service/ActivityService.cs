using PointrCdn.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Web;

namespace PointrCdn.Service
{
    public class ActivityService
    {
        public static void LogException(Exception exception)
        {
            try
            {
                if (exception != null)
                {
                    StackFrame frame = new StackFrame(1);
                    MethodBase methodBase = frame.GetMethod();

                    string methodName = methodBase.Name;
                    string className = methodBase.DeclaringType.Name;

                    string msg = "";
                    int exCount = 1;
                    Exception ex = exception;
                    while (ex != null)
                    {
                        msg += exCount + "\n" + ex.Message + "\n" + ex.StackTrace + "\n\n";
                        ex = ex.InnerException;
                        exCount++;
                    }

                    using (CdnContext context = new CdnContext())
                    {
                        ExceptionLog log = new ExceptionLog()
                        {
                            function = methodName,
                            objectClass = className,
                            exceptionMessage = msg,
                        };
                        context.exceptions.Add(log);
                        context.SaveChanges();
                    }
                }
            }
            catch (Exception ex) { }
        }
    }
}