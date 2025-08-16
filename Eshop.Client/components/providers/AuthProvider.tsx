"use client";

import theme from "@/theme";
import { Navigation } from "@toolpad/core";
import { DashboardLayout } from "@toolpad/core/DashboardLayout";
import { PageContainer } from "@toolpad/core/PageContainer";
import { NextAppProvider } from "@toolpad/core/nextjs";
import CircularProgress from "@mui/material/CircularProgress";
import {
  createContext,
  useCallback,
  useContext,
  useEffect,
  useMemo,
  useState,
} from "react";

import { shouldRefreshTokens } from "@/api/core/api";
import {
  ExtendedSession,
  getSession,
  logout,
  refreshTokens,
} from "@/api/users";
import ToolbarActions from "@/components/ToolbarActions";
import CartProvider from "@/components/providers/CartProvider";

const Branding = {
  title: "Eshop",
};

type UserContextType = {
  isLoading: boolean;
  session: ExtendedSession | null;
  setSession: (session: ExtendedSession) => void;
};

const UserContext = createContext<UserContextType>(null!);
export const useUserContext = () => useContext(UserContext);

export interface AppAuthProviderProps {
  children: React.ReactNode;
  navigation: Navigation;
}

const slots = { toolbarActions: ToolbarActions };

const getRefreshTime = (expirationDate: Date) => {
  const now = Date.now();
  const expiry = new Date(expirationDate).getTime();

  let intervalMs = expiry - 1000 - now;

  return intervalMs < 0 ? 60 * 1000 : intervalMs;
};

export function AuthProvider({ children, navigation }: AppAuthProviderProps) {
  const [isLoading, setIsLoading] = useState(true);
  const [session, setSession] = useState<ExtendedSession | null>(null);
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
      isLoading,
      session,
      setSession,
    }),
    [isLoading, session]
  );

  const authentication = useMemo(
    () =>
      isLoading
        ? undefined
        : {
            signIn,
            signOut,
          },
    [isLoading, signIn, signOut]
  );

  const fetchSession = useCallback(async () => {
    try {
      const data = await getSession();
      setSession(data);
    } catch {
      setSession(null);
    } finally {
      setIsLoading(false);
    }
  }, []);

  useEffect(() => {
    if (session || shouldRefreshTokens()) return;

    fetchSession();
  }, [session, fetchSession]);

  useEffect(() => {
    if (!session?.accessTokenExpirationDate) return;
    const refreshTime = getRefreshTime(session.accessTokenExpirationDate);

    const interval = setInterval(async () => {
      try {
        console.info("Refresh tokens.");
        await refreshTokens();
        await fetchSession();
      } catch (err) {
        console.error("Token refresh failed.", err);
      }
    }, refreshTime);

    return () => clearInterval(interval);
  }, [session, fetchSession]);

  useEffect(() => {
    if (!shouldRefreshTokens()) return;

    const fetch = async () => {
      try {
        console.info("Refresh tokens.");
        await refreshTokens();
        await fetchSession();
      } catch (err) {
        console.error("Token refresh failed.", err);
      }
    };

    fetch();
  }, [fetchSession]);

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
            <PageContainer title="">
              {isLoading ? <CircularProgress /> : children}
            </PageContainer>
          </DashboardLayout>
        </CartProvider>
      </NextAppProvider>
    </UserContext.Provider>
  );
}
