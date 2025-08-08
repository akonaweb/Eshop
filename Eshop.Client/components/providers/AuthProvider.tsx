"use client";

import theme from "@/theme";
import { Navigation, Session } from "@toolpad/core";
import { DashboardLayout } from "@toolpad/core/DashboardLayout";
import { PageContainer } from "@toolpad/core/PageContainer";
import { NextAppProvider } from "@toolpad/core/nextjs";
import {
  createContext,
  useCallback,
  useContext,
  useEffect,
  useMemo,
  useState,
} from "react";

import { getSession, logout, refreshTokens } from "@/api/users";
import ToolbarActions from "@/components/ToolbarActions";
import CartProvider from "@/components/providers/CartProvider";

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

const slots = { toolbarActions: ToolbarActions };

export function AuthProvider({ children, navigation }: AppAuthProviderProps) {
  const [session, setSession] = useState<Session | null>(null);

  const signIn = useCallback(() => (window.location.href = "/login"), []);

  const signOut = useCallback(async () => {
    try {
      await logout();
    } catch (err) {
      console.error("Logout failed.", err);
    } finally {
      window.location.href = "/";
    }
  }, []);

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
    [signIn, signOut]
  );

  useEffect(() => {
    const fetch = async () => {
      try {
        const data = await getSession();
        setSession(data);
      } catch {
        setSession(null);
      }
    };

    if (session) return;
    fetch();
  }, [session]);

  useEffect(() => {
    const interval = setInterval(
      async () => {
        if (!session) return;

        try {
          console.info("Refresh tokens.");
          await refreshTokens();
        } catch (err) {
          console.error("Token refresh failed.", err);
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
