using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using ProselApp.Models;
using ProselApp.Libraries.Lang;
using ProselApp.Services.Interfaces;
using ProselApp.Models.Const;
using ProselApp.Models.AcessCode;
using System;
using ProselApp.Models.ViewModel;

namespace ProselApp.Controllers
{
    public class UserController : Controller
    {
        private readonly ICodeService codeSvc;
        private readonly IConfiguration conf;
        private readonly ILoginService loginSvc;
        private readonly IUserService userSvc;

        public UserController(ICodeService codeSvc, IConfiguration conf, ILoginService loginSvc, IUserService userSvc)
        {
            this.codeSvc = codeSvc;
            this.conf = conf;
            this.loginSvc = loginSvc;
            this.userSvc = userSvc;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet, Route("Login")]
        public IActionResult Login()
        {
            if (loginSvc.GetUser() is null)
            {
                return View();
            }
            return RedirectToAction("Index", "Agendas");
        }

        [HttpPost, Route("Login")]
        public async Task<IActionResult> Login([FromForm] User user)
        {

            User userDB = await userSvc.GetUserByLogin(user.Email, user.Password);

            if (!(userDB is null))
            {
                loginSvc.Login(userDB);
                return RedirectToAction("Index", "Agendas");
            }
            else
            {
                ViewData["MSG_E"] = MSG.MSG_E007;
                return View();
            }
        }
        [HttpGet, Route("Perfil")]
        public async Task<IActionResult> MyProfile()
        {
            return View(await userSvc.GetByCpfAsync(loginSvc.GetUser().Cpf));
        }

        [HttpPost, Route("Perfil")]
        public async Task<IActionResult> MyProfile(User user)
        {
            ModelState.Remove("AcessType");
            ModelState.Remove("AccountStatus");
            ModelState.Remove("Password");
            ModelState.Remove("PasswordConfirmation");
            if (!ModelState.IsValid)
            {
                return RedirectToAction("MyProfile", user);
            }
            await userSvc.UpdateProfileAsync(user);
            return RedirectToAction("Index", "Agendas");
        }

        [HttpGet, Route("Cadastro")]
        public IActionResult CadastroUsuario()
        {
            if (loginSvc.GetUser() is null)
            {
                return View();
            }
            return RedirectToAction("Index", "Agendas");
        }

        [HttpPost, Route("Cadastro")]
        public async Task<IActionResult> CadastroUsuario([FromForm] User User)
        {
            if (ModelState.IsValid)
            {
                await userSvc.AddAsync(User);
                TempData["MSG_S"] = MSG.MSG_S006;
                if (!(User.Email.ToLower() == conf.GetValue<string>("Email:Username").ToLower()))
                {
                    await codeSvc.CreateNewKeyAsync(User, CodeType.Verification);
                }
                return RedirectToAction(nameof(Login));
            }
            return View(User);
        }
        
        [HttpGet]
        public IActionResult RecuperarSenha()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> RecuperarSenha(User user)
        {
            var users = userSvc.GetUserByEmail(user.Email);
            if (users.Count is 0)
            {
                TempData["MSG_E"] = MSG.MSG_E014;
                return View();
            }

            var codigo = new AccessCode { User = new User { Email = user.Email }, CodeType = CodeType.Recovery };
            var elapsedTime = await codeSvc.ElapsedTimeAsync(codigo);
            var _15min = new TimeSpan(0, 15, 0);
            TempData["MSG_E"] = string.Format(MSG.MSG_E011, _15min.Subtract(elapsedTime).ToString(@"mm\:ss"));

            if (elapsedTime >= _15min)
            {
                TempData["MSG_E"] = null;
                await codeSvc.CreateNewKeyAsync(users[0], CodeType.Recovery);
                TempData["MSG_S"] = MSG.MSG_S003;
            }

            user.Cpf = users[0].Cpf;
            return RedirectToAction("NovaSenha", user);
        }

        [HttpGet]
        public IActionResult NovaSenha(User user)
        {
            UserCode viewModel = new UserCode
            {
                User = user
            };
            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> NovaSenha(UserCode usercode)
        {
            ModelState.Remove("user.Nome");
            ModelState.Remove("user.TipoAcesso");
            ModelState.Remove("user.SituacaoConta");
            ModelState.Remove("user.SituacaoPagamento");
            ModelState.Remove("user.NumeroPlano");
            ModelState.Remove("user.Nascimento");
            ModelState.Remove("user.Sexo");
            ModelState.Remove("user.CPF");
            ModelState.Remove("user.DDD");
            ModelState.Remove("user.EnderecoId");
            ModelState.Remove("user.Telefone");


            if (ModelState.IsValid)
            {
                usercode.AccessCode.User.Email = usercode.User.Email;
                usercode.AccessCode.CodeType = CodeType.Recovery;

                if (codeSvc.CodeIsValid(usercode.AccessCode.Key))
                {

                    if (!await userSvc.ChangePasswordByCodeAsync(usercode.User, usercode.AccessCode))
                    {
                        TempData["MSG_E"] = MSG.MSG_E015;
                        usercode.AccessCode.Key = string.Empty;
                        return View(usercode);
                    }

                    TempData["MSG_S"] = MSG.MSG_S005;
                    return RedirectToAction("Login", "users");
                }
            }

            TempData["MSG_E"] = MSG.MSG_E013;
            return View(usercode);
        }

        [HttpGet]
        public IActionResult AtivarConta()
        {
            var loggedUser = loginSvc.GetUser();

            if (loggedUser is null)
            {
                return RedirectToAction("Login", "users");
            }

            if (loggedUser.AccountStatus != true)
            {
                UserCode viewModel = new UserCode
                {
                    User = loggedUser
                };

                return View(viewModel);

            }
            return RedirectToAction("Index", "Agendas");
        }

        [HttpPost]
        public async Task<IActionResult> AtivarConta(UserCode usercode)
        {
            var loggedUser = loginSvc.GetUser();

            if (loggedUser.AccountStatus != true)
            {
                ModelState.Remove("user.Nome");
                ModelState.Remove("user.TipoAcesso");
                ModelState.Remove("user.SituacaoConta");
                ModelState.Remove("user.SituacaoPagamento");
                ModelState.Remove("user.NumeroPlano");
                ModelState.Remove("user.Nascimento");
                ModelState.Remove("user.Sexo");
                ModelState.Remove("user.CPF");
                ModelState.Remove("user.DDD");
                ModelState.Remove("user.EnderecoId");
                ModelState.Remove("user.Telefone");
                ModelState.Remove("user.Senha");
                ModelState.Remove("user.ConfirmacaoSenha");

                if (ModelState.IsValid)
                {
                    usercode.AccessCode.User.Email = usercode.User.Email;
                    usercode.AccessCode.CodeType = CodeType.Verification;

                    if (codeSvc.CodeIsValid(usercode.AccessCode.Key))
                    {
                        var accountEnabled = await userSvc.ActiveAccountAsync(loggedUser, usercode.AccessCode);

                        if (!accountEnabled)
                        {
                            TempData["MSG_E"] = MSG.MSG_E015;
                            return View(usercode);
                        }

                        loginSvc.Login(await userSvc.GetByCpfAsync(loggedUser.Cpf));
                        TempData["MSG_S"] = MSG.MSG_S008;
                        return RedirectToAction("Index", "Agendas");
                    }
                }
                TempData["MSG_E"] = MSG.MSG_E013;
                return View(usercode);
            }
            return RedirectToAction("Index", "Agendas");
        }

        [HttpGet]
        public IActionResult NovoCodigoConfirmacao() => PartialView();

        [HttpPost]
        public async Task<IActionResult> NovoCodigoConfirmacao(string codeType)
        {
            var loggedUser = loginSvc.GetUser();
            if (!(loggedUser is null))
            {
                var codigo = new AccessCode { User = new User { Email = loggedUser.Email }, CodeType = codeType };
                var elapsedTime = await codeSvc.ElapsedTimeAsync(codigo);
                var _15min = new TimeSpan(0, 15, 0);
                TempData["MSG_E"] = string.Format(MSG.MSG_E011, _15min.Subtract(elapsedTime).ToString(@"mm\:ss"));

                if (elapsedTime >= _15min)
                {
                    TempData["MSG_E"] = null;
                    await codeSvc.CreateNewKeyAsync(loggedUser, codeType);
                    TempData["MSG_S"] = MSG.MSG_S007;
                }

                return RedirectToAction("AtivarConta", new UserCode { User = loggedUser });
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult Logout()
        {
            loginSvc.Logout();
            return RedirectToAction(nameof(Index));
        }
        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
