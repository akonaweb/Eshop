import api, {
  accessTokenKey,
  refreshTokenKey,
  removeAccessToken,
  setAccessToken,
} from "./core/api";
import urls from "./core/urls";

export async function login(email: string, password: string) {
  const response = await api().post(urls.user.login, {
    email,
    password,
  });
  setAccessToken(response.data[accessTokenKey]);
}

export async function logout() {
  removeAccessToken();
  await api().post(urls.user.logout);
}

export async function refreshTokens() {
  const response = await api().get(urls.user.refreshTokens);
  setAccessToken(response.data[accessTokenKey]);
}
