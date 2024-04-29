using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SampleSwagger.ViewModels;
using System.Collections.Generic;

namespace SampleSwagger.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    public class DeviceController : ControllerBase
    {
        private readonly List<DeviceVM> _devices = new List<DeviceVM>();
        public DeviceController() {
            _devices = new List<DeviceVM>
            {
                new DeviceVM {Id = 1, Name = "Dev1"},
                new DeviceVM {Id = 2, Name = "Dev2"},
                new DeviceVM {Id = 3, Name = "Dev3"}
            };
        }

        [HttpGet]
        public IActionResult GetDevices()
        {
            return Ok(_devices);
        }

        [HttpPost]
        public IActionResult SaveDevice(DeviceVM device)
        {
            _devices.Add(device);
            return CreatedAtAction(nameof(GetDevice), new { id = device.Id }, device);
        }

        [HttpGet("{id}")]
        public IActionResult GetDevice(int id)
        {
            var device = _devices.Find(d => d.Id == id);
            if (device == null)
            {
                return NotFound();
            }
            return Ok(device);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateDevice(int id, DeviceVM device)
        {
            var index = _devices.FindIndex(d => d.Id == id);
            if (index == -1)
            {
                return NotFound();
            }
            _devices[index] = device;
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteDevice(int id)
        {
            var index = _devices.FindIndex(d => d.Id == id);
            if (index == -1)
            {
                return NotFound();
            }
            _devices.RemoveAt(index);
            return NoContent();
        }
    }
}
