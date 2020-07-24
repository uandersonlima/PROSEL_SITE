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
        private readonly ITokenService tokenSvc;
        private readonly IUserService userSvc;

        public UserController(ICodeService codeSvc, IConfiguration conf, ILoginService loginSvc, ITokenService tokenSvc, IUserService userSvc)
        {
            this.codeSvc = codeSvc;
            this.conf = conf;
            this.loginSvc = loginSvc;
            this.tokenSvc = tokenSvc;
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
            return RedirectToAction("Index", "message");
        }

        [HttpPost, Route("Login")]
        public async Task<IActionResult> Login([FromForm] User user)
        {

            User userDB = await userSvc.GetUserByLogin(user.Email, user.Password);

            if (!(userDB is null))
            {
                loginSvc.Login(userDB);
                return RedirectToAction("Index", "Message");
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
            return RedirectToAction("Index", "message");
        }

        public IActionResult NovoUsuario()
        {
            if (loginSvc.GetUser() != null)
            {
                return RedirectToAction("index", "message");
            }
            return View();
        }

        [HttpPost]
        public IActionResult NovoUsuario(Token token)
        {
            return RedirectToAction("cadastrousuario", "user", token);
        }

        [HttpGet, Route("Cadastro")]
        public async Task<IActionResult> CadastroUsuario(Token token)
        {
            if (loginSvc.GetUser() != null)
                return RedirectToAction("index", "message");
            if (token == null || !await tokenSvc.CheckTokenAsync(token))
            {
                TempData["MSG_E"] = MSG.MSG_E017;
                return RedirectToAction("novousuario");
            }
            return View(new UserToken { Token = token });
        }

        [HttpPost, Route("Cadastro")]
        public async Task<IActionResult> CadastroUsuario(UserToken usertoken)
        {
            if (ModelState.IsValid)
            {
                var token = await tokenSvc.GetByHashAsync(usertoken.Token.SecurityToken);
                token.UserCpf = usertoken.User.Cpf;
                await userSvc.AddAsync(usertoken.User);
                await tokenSvc.UpdateAsync(token);
                TempData["MSG_S"] = MSG.MSG_S006;
                if (!(usertoken.User.Email.ToLower() == conf.GetValue<string>("Email:Username").ToLower()))
                {
                    await codeSvc.CreateNewKeyAsync(usertoken.User, CodeType.Verification);
                }
                return RedirectToAction(nameof(Login));
            }
            return View(usertoken);
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
            ModelState.Remove("user.Name");
            ModelState.Remove("user.Cpf");
            ModelState.Remove("user.Telephone");

            if (ModelState.IsValid)
            {
                usercode.AccessCode.User = usercode.User;
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
                    return RedirectToAction("login", "user");
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
                return RedirectToAction("login", "user");
            }

            if (loggedUser.AccountStatus != true)
            {
                UserCode viewModel = new UserCode
                {
                    User = loggedUser
                };

                return View(viewModel);

            }
            return RedirectToAction("index", "message");
        }

        [HttpPost]
        public async Task<IActionResult> AtivarConta(UserCode usercode)
        {
            var loggedUser = loginSvc.GetUser();

            if (loggedUser.AccountStatus != true)
            {
                ModelState.Remove("user.Name");
                ModelState.Remove("user.Cpf");
                ModelState.Remove("user.Telephone");
                ModelState.Remove("user.Password");
                ModelState.Remove("user.PasswordConfirmation");

                if (ModelState.IsValid)
                {
                    usercode.AccessCode.User = usercode.User;
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
                        return RedirectToAction("index", "message");
                    }
                }
                TempData["MSG_E"] = MSG.MSG_E013;
                return View(usercode);
            }
            return RedirectToAction("Index", "message");
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

        [HttpGet]
        public async Task<IActionResult> Users()
        {
            return View(await userSvc.GetAllUserAsync());
        }
        [HttpGet]
        public async Task<IActionResult> DeleteUser(string cpf)
        {
            if (string.IsNullOrEmpty(cpf))
            {
                return NotFound();
            }
            var user = await userSvc.GetByCpfAsync(cpf);
            if (user == null)
            {
                return NotFound();
            }
            return PartialView(user);
        }
        [HttpPost]
        public async Task<IActionResult> Delete(string cpf)
        {
            if (string.IsNullOrEmpty(cpf))
            {
                return NotFound();
            }
            var user = await userSvc.GetByCpfAsync(cpf);
            if (user == null)
            {
                return NotFound();
            }
            return PartialView(user);
        }

    }
}
