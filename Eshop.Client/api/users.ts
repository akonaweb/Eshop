import { getClientAccessToken } from "./accessToken";
import api from "./api";
import urls from "./urls";

export async function login(email: string, password: string) {
  const response = await api(getClientAccessToken()).post(urls.user.login, {
    email,
    password,
  });

  localStorage.setItem("accessToken", response.data.accessToken);

  return response.data;
}

export async function logout() {
  localStorage.removeItem("accessToken");
  const response = await api().post(urls.user.logout);
  return response.data;
}

export async function refreshTokens() {
  const response = await api().get(urls.user.refreshTokens);
  localStorage.setItem("accessToken", response.data.accessToken);
  return response.data;
}
