using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class PlayFabLogin : MonoBehaviour
{
    [Header("Message")]
    [SerializeField] TMP_Text MsgSuccess;
    [SerializeField] TMP_Text MsgFailed;

    [Header("Login")]
    [SerializeField] TMP_InputField EmailLoginInput;
    [SerializeField] TMP_InputField PasswordLoginInput;
    [SerializeField] GameObject Login;

    [Header("Register")]
    [SerializeField] TMP_InputField UsernameRegisterInput;
    [SerializeField] TMP_InputField EmailRegisterInput;
    [SerializeField] TMP_InputField PasswordRegisterInput;
    [SerializeField] TMP_InputField PasswordRegisterInput2;
    [SerializeField] GameObject Register;

    [Header("Recovery")]
    [SerializeField] TMP_InputField EmailRecoveryInput;
    [SerializeField] GameObject Recovery;

    private void ShowNotificationMessage(string message)
    {
        OpenLogin();
        MsgFailed.text = message;
    }

    public void LoginUser()
    {
        if (string.IsNullOrEmpty(EmailLoginInput.text) && string.IsNullOrEmpty(PasswordLoginInput.text))
        {
            ShowNotificationMessage("Campos vacios\nIntroduce todos los datos");
            return;
        }
        
        if (PasswordLoginInput.text.Length < 6)
        {
            ShowNotificationMessage("Introduce al menos 6 caracteres en la contraseña");
            return;
        }

        var request = new LoginWithEmailAddressRequest
        {
            Email = EmailLoginInput.text,
            Password = PasswordLoginInput.text,

            InfoRequestParameters = new PlayFab.ClientModels.GetPlayerCombinedInfoRequestParams
            {
                GetPlayerProfile = true
            }
        };
        PlayFabClientAPI.LoginWithEmailAddress(request, OnLoginSuccess, OnError);
    }

    private void OnLoginSuccess(LoginResult result)
    {
        string displayName = null;
        if (result.InfoResultPayload is not null) 
        {
            displayName = result.InfoResultPayload.PlayerProfile.DisplayName;
        }
        GameManager.Instance.Username = displayName;
        SceneManager.LoadScene("Menu");
    }

    public void RegisterUser()
    {
        if (string.IsNullOrEmpty(EmailRegisterInput.text) && string.IsNullOrEmpty(PasswordRegisterInput.text) && string.IsNullOrEmpty(PasswordRegisterInput2.text) && string.IsNullOrEmpty(UsernameRegisterInput.text))
        {
            ShowNotificationMessage("Campos vacios\nIntroduce todos los datos");
            return;
        }

        if (PasswordRegisterInput.text.Length < 6)
        {
            ShowNotificationMessage("Introduce al menos 6 caracteres en la contraseña");
            return;
        }

        if (!PasswordRegisterInput.text.Equals(PasswordRegisterInput2.text))
        {
            ShowNotificationMessage("Las contraseñas no coinciden");
            return;
        }

        var request = new RegisterPlayFabUserRequest
        {
            DisplayName = UsernameRegisterInput.text,
            Email = EmailRegisterInput.text,
            Password = PasswordRegisterInput.text,

            RequireBothUsernameAndEmail = false,
        };

        PlayFabClientAPI.RegisterPlayFabUser(request, OnSignUpSuccess, OnError);
    }

    private void OnSignUpSuccess(RegisterPlayFabUserResult result)
    {
        MsgSuccess.text = "¡Cuenta creada exitosamente!";
        OpenLogin();
    }

    private void OnError(PlayFabError error)
    {
        ShowNotificationMessage(error.ErrorMessage);
        Debug.Log(error.GenerateErrorReport());
    }

    public void RecoverUser()
    {
        if (string.IsNullOrEmpty(EmailRecoveryInput.text))
        {
            ShowNotificationMessage("Porfavor, introduce tu email.");
            return;
        }
        //Recover user
        var request = new SendAccountRecoveryEmailRequest
        {
            Email = EmailRecoveryInput.text,
            TitleId = "DA735",
        };
        PlayFabClientAPI.SendAccountRecoveryEmail(request, OnRecoverySuccess, OnError);
    }

    private void OnRecoverySuccess(SendAccountRecoveryEmailResult result)
    {
        OpenLogin();
        ShowNotificationMessage("Recovery mail sent.");
    }

    public void OpenLogin()
    {
        Login.SetActive(true);
        Register.SetActive(false);
        Recovery.SetActive(false);
    }


}