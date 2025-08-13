import React from "react";
import { useFormContext, Path, FieldValues } from "react-hook-form";
import MuiTextField from "@mui/material/TextField";

type TextProps<T extends FieldValues> = {
  name: Path<T>;
  label?: string;
  type?: string;
} & Omit<React.ComponentProps<typeof MuiTextField>, "name" | "defaultValue">;
function Text<T extends FieldValues>({ name, label, ...rest }: TextProps<T>) {
  const {
    register,
    formState: { errors },
  } = useFormContext<T>();

  const errorMessage = errors[name]?.message as string | undefined;

  return (
    <MuiTextField
      {...register(name)}
      label={label}
      type="text"
      error={!!errorMessage}
      helperText={errorMessage}
      size="small"
      fullWidth
      {...rest}
    />
  );
}

export default Text;
