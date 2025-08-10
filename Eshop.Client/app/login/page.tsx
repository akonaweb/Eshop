"use client";

import { CircularProgress } from "@mui/material";
import { SignInPage, type AuthProvider } from "@toolpad/core/SignInPage";
import { useCallback, useEffect, useState } from "react";

import { login, logout, refreshTokens } from "@/api/users";
import { useUserContext } from "@/components/providers/AuthProvider";
import { useSearchParams } from "next/navigation";

const providers = [{ id: "credentials", name: "Email and Password" }];
const signIn: (provider: AuthProvider, formData: FormData) => void = async (
  _,
  formData
) => {
  const promise = new Promise<void>(async (resolve) => {
    const email = formData.get("email") as string;
    const password = formData.get("password") as string;

    try {
      await login(email, password);
      window.location.href = "/";
    } catch {
      try {
        await logout();
      } catch {
        console.error("Something went wrong!");
      }
      resolve({ error: "Login failed!" } as any);
    }
  });

  return promise;
};

export default function LoginPage() {
  const [isLoading, setIsLoading] = useState(true);
  const { session } = useUserContext();
  const searchParams = useSearchParams();
  const backUrl = searchParams.get("back-url") ?? "";

  useEffect(() => {
    const fetch = async () => {
      try {
        if (!backUrl) return;

        console.info("Refresh tokens.");
        await refreshTokens();
        window.location.href = backUrl;
        // todo redirect to back-url
      } catch (err) {
        window.location.href = "/login";
        console.error("Token refresh failed.", err);
      }
    };

    fetch();
  }, [backUrl]);

  useEffect(() => {
    if (backUrl) return;
    setIsLoading(false);
    if (session) window.location.href = "/";
  }, [backUrl, session]);

  if (isLoading) return <CircularProgress />;

  return (
    <SignInPage
      signIn={signIn}
      providers={providers}
      slotProps={{
        emailField: { autoFocus: true },
        form: { noValidate: true },
        rememberMe: {},
      }}
    />
  );
}
