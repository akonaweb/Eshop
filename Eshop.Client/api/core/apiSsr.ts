import axios from "axios";
import { redirect } from "next/navigation";

export type ProblemDetails = {
  title: string;
  status: number;
  detail?: string;
  errors?: Record<string, string[]>;
};

const apiSsr = (accessToken: string, backUrl: string) => {
  const instance = axios.create({
    baseURL: process.env.NEXT_PUBLIC_API_URL,
    headers: {
      Authorization: `Bearer ${accessToken}`,
    },
  });

  instance.interceptors.response.use(
    (res) => res,
    async (error) => {
      const originalRequest = error.config;

      if (error.response?.status === 401 && !originalRequest._retry) {
        originalRequest._retry = true;
        redirect(`/login?back-url=${backUrl}`);
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

export default apiSsr;
