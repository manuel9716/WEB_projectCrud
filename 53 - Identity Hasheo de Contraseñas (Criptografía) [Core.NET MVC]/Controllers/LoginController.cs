using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using SimpleLogin.Helper;
using SimpleLogin.Models;
using SimpleLogin.Models.ViewModel;

namespace SimpleLogin.Controllers
{
    public class LoginController : Controller
    {
        CodeStackCTX ctx;

        public LoginController(CodeStackCTX _ctx)
        {
            ctx = _ctx;
        }

        public IActionResult Index()
        {
            return View();
        }

        [BindProperty]
        public UsuarioVM Usuario {get; set;}
        public async Task<IActionResult> Login()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new JObject() {
                    { "StatusCode", 400 },
                    { "Message", "El usuario ya existe, seleccione otro." }
                });
            }
            else
            {
                var result = await ctx.Usuarios.Where(x => x.Nombre == Usuario.Nombre).SingleOrDefaultAsync();
                if (result == null)
                {
                    return NotFound(new JObject() {
                        { "StatusCode", 404 },
                        { "Message", "Usuario no encontrado." }
                    });
                }
                else
                {
                    if (HashHelper.CheckHash(Usuario.Clave, result.Clave, result.Sal))
                    {
                        var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme, ClaimTypes.Name, ClaimTypes.Role);
                        identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, result.IdUsuario.ToString()));
                        identity.AddClaim(new Claim(ClaimTypes.Name, result.Nombre));
                        identity.AddClaim(new Claim(ClaimTypes.Email, "jose.jairo.fuentes@gmail.com"));
                        identity.AddClaim(new Claim("Dato", "Valor"));
                        var principal = new ClaimsPrincipal(identity);
                        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal,
                            new AuthenticationProperties { ExpiresUtc = DateTime.Now.AddDays(2), IsPersistent = true });
                        return Ok(result);
                    }
                    else
                    {
                        var response = new JObject() {
                            { "StatusCode", 403 },
                            { "Message", "Usuario o contraseña no válida." }
                        };
                        return StatusCode(403, response);
                    }
                }
            }
        }

        public async Task<IActionResult> Logout()
        {
           await HttpContext.SignOutAsync();
           return Redirect("/Login");
        }
    }
}