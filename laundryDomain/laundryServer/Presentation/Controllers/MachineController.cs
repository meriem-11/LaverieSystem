using laundryServer.Business.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace laundryServer.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MachineController : ControllerBase
    {
        private readonly IMachineService _machineService;

        public MachineController(IMachineService machineService)
        {
            _machineService = machineService;
        }

        [HttpGet]
        public IActionResult GetMachines()
        {
            var machines = _machineService.GetMachines();
            return Ok(machines);
        }

        [HttpGet("{id}")]
        public IActionResult GetMachineById(int id)
        {
            var machine = _machineService.GetMachineById(id);
            if (machine == null) return NotFound();
            return Ok(machine);
        }
    }
}
