﻿namespace ULog.MongoDb.Entries;
public class UResponseEntry
{
    public DateTime? DateTime { get; set; }
    public double SecondDiff { get; set; }
    public string? Message { get; set; }
    public int StatusCode { get; set; }
}
