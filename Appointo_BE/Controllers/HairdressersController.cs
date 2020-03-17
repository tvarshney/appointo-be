﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Appointo_BE.DTOs;
using Appointo_BE.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Appointo_BE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HairdressersController : ControllerBase
    {
        private readonly IHairdresserRepository _hairdresserRepository;

        public HairdressersController(IHairdresserRepository repo)
        {
            _hairdresserRepository = repo;
        }

        [HttpGet]
        public IEnumerable<Hairdresser> GetHairdressers(string name = null, string location = null)
        {
            if (string.IsNullOrEmpty(name) && string.IsNullOrEmpty(location))
                return _hairdresserRepository.GetAll();
            else return _hairdresserRepository.GetBy(name, location);
        }

        [HttpGet("{id}")]
        public ActionResult<Hairdresser> GetHairdresser(int id)
        {
            Hairdresser hairdresser = _hairdresserRepository.GetBy(id);
            if (hairdresser == null) return NotFound();
            return hairdresser;
        }

        [HttpPost]
        public ActionResult<Hairdresser> PostHairdresser(HairdresserDTO hairdresser)
        {
            Hairdresser hairdresserToCreate = new Hairdresser() { Name = hairdresser.Name };
            foreach (var i in hairdresser.Treatments)
                hairdresserToCreate.AddTreatment(new Treatment(i.Name, new TimeSpan(i.Duration.Hours, i.Duration.Minutes, i.Duration.Seconds)));

            _hairdresserRepository.Add(hairdresserToCreate);
            _hairdresserRepository.SaveChanges();

            return CreatedAtAction(nameof(GetHairdresser), new { hairdresserToCreate.Id }, hairdresserToCreate);
        }
    }
}   