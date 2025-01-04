using laundryHeart.Domain.Entities;
using laundryServer.Business.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace laundryServer.Presentation.Controllers

{
    [Route("api/[controller]")]
    [ApiController]
    public class MachineController : ControllerBase
    {
        private readonly IMachineService _machineService;

        // Injection de dépendance du service
        public MachineController(IMachineService machineService)
        {
            _machineService = machineService;
        }

        // GET: api/Machine
        [HttpGet]
        public ActionResult<List<Machine>> GetMachines()
        {
            return Ok(_machineService.GetMachines());
        }

        // GET: api/Machine/5
        [HttpGet("{id}")]
        public ActionResult<Machine> GetMachineById(int id)
        {
            var machine = _machineService.GetMachineById(id);
            if (machine == null)
            {
                return NotFound();
            }
            return Ok(machine);
        }

        // PUT: api/Machine/5/status
        [HttpPut("{id}/status")]
        public IActionResult UpdateMachineStatus(int id, [FromBody] bool isAvailable)
        {
            _machineService.UpdateMachineStatus(id, isAvailable);
            return NoContent();
        }
    }
}
