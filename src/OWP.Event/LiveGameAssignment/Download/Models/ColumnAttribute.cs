namespace OWP.Event.LiveGameAssignment.Download.Models;

using System;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Property)]
public sealed class ColumnAttribute : Attribute
{
    public ColumnAttribute(ColumnName name)
    {
        Name = name;
    }

    public ColumnName Name { get; }
}