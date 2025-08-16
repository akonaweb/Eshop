import axios from "axios";

import urls from "./urls";

export const accessTokenKey = "accessToken";

const checkCookie = (name: string) => {
  const cookies = document.cookie.split(";").map((c) => c.trim());
  return cookies.some((c) => c.startsWith(name + "="));
};

export const shouldRefreshTokens = () => {
  return !checkCookie(accessTokenKey) && checkCookie("refreshToken");
};

export type ProblemDetails = {
  title: string;
  status: number;
  detail?: string;
  errors?: Record<string, string[]>;
};

const api = axios.create({
  baseURL: process.env.NEXT_PUBLIC_API_URL,
  withCredentials: true,
});

api.interceptors.response.use(
  (res) => res,
  async (error) => {
    const originalRequest = error.config;

    if (error.response?.status === 401 && !originalRequest._retry) {
      originalRequest._retry = true;

      try {
        await axios.get(
          `${process.env.NEXT_PUBLIC_API_URL}/${urls.user.refreshTokens}`,
          { withCredentials: true }
        );

        window.location.href = window.location.href;
      } catch (refreshErr) {
        console.warn("Refresh token failed", refreshErr);
        window.location.href = "/login";
      }
    }

    if (error.response?.status === 403) {
      console.warn("Access forbidden – redirecting to /forbidden");
      window.location.href = "/forbidden";
    }

    throw error.response?.data as ProblemDetails;
  }
);

export default api;
