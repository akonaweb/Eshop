import { cookies } from "next/headers";

import { accessTokenKey } from "./api";

export const getSsrAccessToken = async () => {
  const cookieStore = cookies();
  return (await cookieStore).get(accessTokenKey)?.value!;
};
