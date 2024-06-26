﻿using MattCanello.NewsFeed.SearchApi.Domain.Commands;
using MattCanello.NewsFeed.SearchApi.Domain.Interfaces;
using MattCanello.NewsFeed.SearchApi.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace MattCanello.NewsFeed.SearchApi.Controllers
{
    [ApiController]
    public class DocumentController : ControllerBase
    {
        private readonly IIndexApp _indexApp;
        private readonly IDocumentRepository _documentRepository;

        public DocumentController(IIndexApp indexApp, IDocumentRepository documentRepository)
        {
            _indexApp = indexApp;
            _documentRepository = documentRepository;
        }

        [HttpPost("index")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(Document))]
        public async Task<IActionResult> Index([FromBody, Required] IndexEntryCommand command, CancellationToken cancellationToken = default)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var id = await _indexApp.IndexAsync(command, cancellationToken);

            var doc = new Document(id, command.FeedId!, command.Entry!);

            return CreatedAtAction("GetById", "Document", new { feedId = command.FeedId, id }, doc);
        }

        [ResponseCache(Duration = 3600)]
        [HttpGet("feed/{feedId}/document/{id}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Document))]
        public async Task<IActionResult> GetById([FromRoute, Required] string feedId, [FromRoute, Required] string id, CancellationToken cancellationToken = default)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var doc = await _documentRepository.GetByIdAsync(feedId, id, cancellationToken);

            return Ok(doc);
        }
    }
}
