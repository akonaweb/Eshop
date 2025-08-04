"use client";

import { CircularProgress } from "@mui/material";
import { SignInPage, type AuthProvider } from "@toolpad/core/SignInPage";
import { useEffect, useState } from "react";

import { getAccessToken } from "@/api/core/api";
import { login, logout } from "@/api/users";

const providers = [{ id: "credentials", name: "Email and Password" }];

export default function LoginPage() {
  const [isLoading, setIsLoading] = useState(true);

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

  useEffect(() => {
    if (getAccessToken()) window.location.href = "/";
    else setIsLoading(false);
  }, []);

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
