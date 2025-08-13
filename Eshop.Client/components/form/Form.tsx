import {
  DefaultValues,
  FieldValues,
  FormProvider,
  SubmitHandler,
  useForm,
} from "react-hook-form";

type FormProps<T extends FieldValues> = {
  defaultValues: DefaultValues<T>;
  onSubmit: SubmitHandler<T>;
  children: (methods: ReturnType<typeof useForm<T>>) => React.ReactNode;
};

export function Form<T extends FieldValues>({
  defaultValues,
  onSubmit,
  children,
}: FormProps<T>) {
  const methods = useForm<T>({ defaultValues });

  return (
    <FormProvider {...methods}>
      <form onSubmit={methods.handleSubmit(onSubmit)}>{children(methods)}</form>
    </FormProvider>
  );
}
