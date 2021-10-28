﻿using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApiTest.Services.UtilsService;

namespace WebApiTest.Controllers
{
    public class UtilsController : APIControllerBase
    {
        private readonly IUtilsService _utilsService;
        public UtilsController(IUtilsService utilsService)
        {
            _utilsService = utilsService;
        }

        [HttpPut("Logging/{id}")]
        public async Task<ActionResult> ChangeLogging(int level)
        {
            var result = await _utilsService.ChangeLogLevel(level);
            if (!result.Successfully) return BadRequest();
            return Ok();
        }



    }
}
