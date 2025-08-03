"use client";

import { CircularProgress } from "@mui/material";
import { SignInPage, type AuthProvider } from "@toolpad/core/SignInPage";
import { useEffect, useState } from "react";

import { login } from "@/api/users";

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
      resolve({ error: "Login failed!" } as any);
    }
  });

  return promise;
};

export default function LoginPage() {
  const [isLoading, setIsLoading] = useState(true);

  useEffect(() => {
    const accessToken = localStorage.getItem("accessToken");
    if (accessToken) window.location.href = "/";
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
