using System;
using System.IO;
using System.Net;

namespace AdventOfCode.Utils
{
  class InputGrabber
  {
    public static bool TryFetchInput(int year, int day)
    {
      DateTime est = DateTime.Now.ToUniversalTime().AddHours(-5);

      try
      {
        if (est >= new DateTime(year, 12, day))
        {
          string cookie = string.Empty;

          // Check the config file for a cookie first
          if (IOUtils.ConfigObject().ContainsKey("cookie"))
          {
            cookie = (string)IOUtils.ConfigObject()["cookie"];
          }
          else
          {
            Console.Error.WriteLine("Please add cookie to config.json.");
            throw new Exception();
          }

          WebClient client = new WebClient();
          client.Headers.Add(HttpRequestHeader.Cookie, cookie);
          File.WriteAllText(IOUtils.InputPath(year, day), client.DownloadString(IOUtils.InputURL(year, day)));
        }
        else
        {
          Console.Error.WriteLine("Can't request input before the puzzle is released!");
          throw new Exception();
        }
      }
      catch (Exception)
      {
        Console.Error.WriteLine("Unable to retrieve input.");
        return false;
      }

      return true;
    }
  }
}
