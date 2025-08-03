export type Session = {
  user: { email: string; name: string };
};

export const getClientAccessToken = (): string | null => {
  if (typeof window === "undefined") return null;
  const match = document.cookie.match(/(?:^|; )accessToken=([^;]*)/);
  return match ? decodeURIComponent(match[1]) : null;
};

export const parseJwt = (token: string | null): Session | null => {
  if (!token) return null;

  try {
    const base64Url = token.split(".")[1];
    const base64 = base64Url.replace(/-/g, "+").replace(/_/g, "/");
    const jsonPayload = decodeURIComponent(
      atob(base64)
        .split("")
        .map((c) => "%" + ("00" + c.charCodeAt(0).toString(16)).slice(-2))
        .join("")
    );
    const payload = JSON.parse(jsonPayload);

    const session: Session = {
      user: {
        email: payload.email,
        name:
          payload[
            "http://schemas.microsoft.com/ws/2008/06/identity/claims/role"
          ] ?? "User",
      },
    };

    return session;
  } catch (e) {
    console.error("Failed to parse token", e);
    return null;
  }
};
