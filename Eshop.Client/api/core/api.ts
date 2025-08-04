import axios from "axios";
import urls from "./urls";
import { redirect } from "next/navigation";

export const accessTokenKey = "accessToken";
export const refreshTokenKey = "refreshToken";
export const getAccessToken = () => localStorage.getItem(accessTokenKey);
export const setAccessToken = (value: string) =>
  localStorage.setItem(accessTokenKey, value);
export const removeAccessToken = () => {
  localStorage.removeItem(accessTokenKey);
  document.cookie = `${accessTokenKey}=; path=/; expires=Thu, 01 Jan 1970 00:00:00 UTC; SameSite=None; Secure`;
  document.cookie = `${refreshTokenKey}=; path=/; expires=Thu, 01 Jan 1970 00:00:00 UTC; SameSite=None; Secure`;
};

export type ProblemDetails = {
  title: string;
  status: number;
  detail?: string;
  errors?: Record<string, string[]>;
};

const api = (accessToken?: string | null) => {
  const instance = axios.create({
    baseURL: process.env.NEXT_PUBLIC_API_URL,
    withCredentials: true,
  });

  instance.interceptors.request.use((config) => {
    if (accessToken) config.headers.Authorization = `Bearer ${accessToken}`;
    return config;
  });

  instance.interceptors.response.use(
    (res) => res,
    async (error) => {
      const originalRequest = error.config;

      if (
        error.response?.status === 401 &&
        !originalRequest._retry &&
        !originalRequest.url.includes(urls.user.refreshTokens)
      ) {
        originalRequest._retry = true;

        try {
          const response = await axios.get(
            `${process.env.NEXT_PUBLIC_API_URL}/${urls.user.refreshTokens}`,
            { withCredentials: true }
          );

          const refreshedAccessToken = response.data[accessTokenKey];
          setAccessToken(refreshedAccessToken);
          originalRequest.headers.Authorization = `Bearer ${refreshedAccessToken}`;
          return instance(originalRequest);
        } catch (refreshErr) {
          console.warn("Refresh token failed", refreshErr);
          redirect("/login");
        }
      }

      if (error.response?.status === 403) {
        console.warn("Access forbidden – redirecting to /forbidden");
        redirect("/forbidden");
      }

      throw error.response?.data as ProblemDetails;
    }
  );

  return instance;
};

export default api;
