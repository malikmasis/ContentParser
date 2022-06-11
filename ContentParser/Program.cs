namespace ContentParser;

using System;


public class Program
{
    protected Program()
    {

    }
    static void Main(string[] args)
    {
        try
        {
            ContentParser parser = new();
            Task.FromResult(parser.ParseAsync());
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
}







