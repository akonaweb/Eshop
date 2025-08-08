import { Session } from "@toolpad/core";

import api from "./core/api";
import urls from "./core/urls";

export type ExtendedSession = Session & {
  accessTokenExpirationDate: Date;
};
export async function getSession(): Promise<ExtendedSession | null> {
  const { data } = await api.get("/user/me");
  return data
    ? {
        user: {
          id: data.id,
          email: data.email,
          name: data.role,
        },
        accessTokenExpirationDate: data.accessTokenExpirationDate,
      }
    : null;
}

export async function login(email: string, password: string) {
  await api.post(urls.user.login, {
    email,
    password,
  });
}

export async function logout() {
  await api.post(urls.user.logout);
}

export async function refreshTokens() {
  await api.get(urls.user.refreshTokens);
}
