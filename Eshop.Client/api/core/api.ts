import axios from "axios";
import { redirect } from "next/navigation";

import urls from "./urls";

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

        return api(originalRequest);
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

export default api;
