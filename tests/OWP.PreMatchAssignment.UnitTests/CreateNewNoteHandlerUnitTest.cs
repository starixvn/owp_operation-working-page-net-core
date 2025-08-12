using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using NSubstitute;
using OWP.PrematchAssignment.QueryAndSave.Handlers;
using OWP.PrematchAssignment.QueryAndSave.Models;
using OWP.PrematchAssignment.QueryAndSave.Services;
using Xunit;

namespace OWP.PreMatchAssignment.UnitTests;

public class CreateNewNoteHandlerUnitTest
{
    [Fact]
    public async Task Handle_ShouldCallNoteServiceCreateNewNote_AndReturnNote()
    {
        // Arrange
        var noteService = Substitute.For<NoteService>();
        var handler = new CreateNewNoteHandler(noteService);

        var request = new CreateNewNoteRequest
        {
            OWPLeagueId = 123,
            InfoSiteMatchId = 456,
            Message = "Test note message",
            EventDate = new DateTime(2024, 1, 15)
        };

        var expectedNote = new Note
        {
            PreMatchNoteId = 789,
            Message = "Test note message",
            CreatedDate = DateTime.Now
        };

        noteService.CreateNewNote(request).Returns(expectedNote);

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        await noteService.Received(1).CreateNewNote(request);
        Assert.Equal(expectedNote, result);
    }

    [Fact]
    public async Task Handle_WithNullIds_ShouldCallNoteServiceWithNullValues()
    {
        // Arrange
        var noteService = Substitute.For<NoteService>();
        var handler = new CreateNewNoteHandler(noteService);

        var request = new CreateNewNoteRequest
        {
            OWPLeagueId = null,
            InfoSiteMatchId = null,
            Message = "Note without IDs",
            EventDate = new DateTime(2024, 1, 16)
        };

        var expectedNote = new Note
        {
            PreMatchNoteId = 999,
            Message = "Note without IDs",
            CreatedDate = DateTime.Now
        };

        noteService.CreateNewNote(request).Returns(expectedNote);

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        await noteService.Received(1).CreateNewNote(request);
        Assert.Equal(expectedNote, result);
    }

    [Fact]
    public async Task Handle_WithEmptyMessage_ShouldCallNoteServiceWithEmptyMessage()
    {
        // Arrange
        var noteService = Substitute.For<NoteService>();
        var handler = new CreateNewNoteHandler(noteService);

        var request = new CreateNewNoteRequest
        {
            OWPLeagueId = 111,
            InfoSiteMatchId = 222,
            Message = "",
            EventDate = new DateTime(2024, 1, 17)
        };

        var expectedNote = new Note
        {
            PreMatchNoteId = 333,
            Message = "",
            CreatedDate = DateTime.Now
        };

        noteService.CreateNewNote(request).Returns(expectedNote);

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        await noteService.Received(1).CreateNewNote(request);
        Assert.Equal(expectedNote, result);
    }
}