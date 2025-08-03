import axios from "axios";
import { redirect } from "next/navigation";
import urls from "./urls";

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

          localStorage.setItem("accessToken", response.data.accessToken);
          originalRequest.headers.Authorization = `Bearer ${response.data.accessToken}`;
          return instance(originalRequest);
        } catch (refreshErr) {
          console.error("Refresh token failed", refreshErr);
          redirect("/login");
        }
      }

      if (error.response?.status === 403) {
        console.warn("Access forbidden – redirecting to /forbidden");
        redirect("/forbidden");
      }

      return Promise.reject(error);
    }
  );

  return instance;
};

export default api;
