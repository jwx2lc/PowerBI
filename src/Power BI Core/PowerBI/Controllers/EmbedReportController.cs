﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PowerBI.Models.Embeddings;
using PowerBI.Services.Power_BI.Interfaces;
using PowerBI.Services.Reporting.Interfaces;

namespace PowerBI.Controllers
{
    [Route("api/reports")]
    [ApiController]
    public class EmbedReportController : ControllerBase
    {
        private readonly ILogger<EmbedReportController> _logger;
        private readonly IReportingService _reportingService;

        public EmbedReportController(ILogger<EmbedReportController> logger, IReportingService reportingService)
        {
            _logger = logger;
            _reportingService = reportingService;
        }

        [HttpGet]
        [Route("selection")]
        [Authorize]
        public async Task<Dictionary<string, string>> GetReportsAsync()
        {
            return await _reportingService.GetReportsAsync(User.Identity.Name);
        }

        [HttpGet]
        [Route("embed/{reportId}")]
        public async Task<EmbedReport> GetEmbedReportAsync(int reportId)
        {
            return await _reportingService.GetEmbedReportAsync(reportId, User.Identity.Name);
        }
    }
}