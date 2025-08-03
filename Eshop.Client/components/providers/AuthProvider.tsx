"use client";

import theme from "@/theme";
import { Navigation } from "@toolpad/core";
import { DashboardLayout } from "@toolpad/core/DashboardLayout";
import { PageContainer } from "@toolpad/core/PageContainer";
import { NextAppProvider } from "@toolpad/core/nextjs";
import { createContext, useContext, useEffect, useMemo, useState } from "react";

import { parseJwt, Session } from "@/api/accessToken";
import { logout, refreshTokens } from "@/api/users";
import ToolbarActions from "@/components/ToolbarActions";
import CartProvider from "@/components/providers/CartProvider";
import { redirect } from "next/navigation";

const Branding = {
  title: "Eshop",
};

type UserContextType = {
  session: Session | null;
  setSession: (session: Session) => void;
};

const UserContext = createContext<UserContextType>(null!);
export const useUserContext = () => useContext(UserContext);

export interface AppAuthProviderProps {
  children: React.ReactNode;
  navigation: Navigation;
}

const signIn = () => {
  redirect("/login");
};

const signOut = async () => {
  try {
    document.cookie =
      "accessToken=; path=/; expires=Thu, 01 Jan 1970 00:00:00 UTC; SameSite=None; Secure";
    document.cookie =
      "refreshToken=; path=/; expires=Thu, 01 Jan 1970 00:00:00 UTC; SameSite=None; Secure";

    await logout();
  } catch (err) {
    console.error("Logout failed", err);
  } finally {
    redirect("/");
  }
};

const slots = { toolbarActions: ToolbarActions };

export function AuthProvider({ children, navigation }: AppAuthProviderProps) {
  const [session, setSession] = useState<Session | null>(null!);

  const value = useMemo(
    () => ({
      session,
      setSession,
    }),
    [session]
  );

  const authentication = useMemo(
    () => ({
      signIn,
      signOut,
    }),
    []
  );

  useEffect(() => {
    setSession(parseJwt(localStorage.getItem("accessToken")));
  }, []);

  useEffect(() => {
    const interval = setInterval(
      async () => {
        if (!session) return;

        try {
          console.info("Tokens refresh");
          await refreshTokens();
        } catch (err) {
          console.error("Token refresh failed", err);
        }
      },
      29 * 60 * 1000
    ); // 29 minutes

    return () => clearInterval(interval);
  }, [session]);

  return (
    <UserContext.Provider value={value}>
      <NextAppProvider
        navigation={navigation}
        branding={Branding}
        theme={theme}
        session={session}
        authentication={authentication}
      >
        <CartProvider>
          <DashboardLayout
            sidebarExpandedWidth={300}
            slots={slots}
            disableCollapsibleSidebar
          >
            <PageContainer title="">{children}</PageContainer>
          </DashboardLayout>
        </CartProvider>
      </NextAppProvider>
    </UserContext.Provider>
  );
}
